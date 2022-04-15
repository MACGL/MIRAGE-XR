﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirageXR
{
    public class AudioPlayer : MirageXRPrefab
    {
        [Tooltip ("Audio file. Only .wav format supported for external sources. Internally, .mp3 are supported as well")]
        public string audioName = "audio.wav";
        [Tooltip ("Set to false to read from project's 'Resources' folder; set to true to read from applications 'LocalState' folder on HoloLens, or online, if filename starts with 'http'")]
        public bool useExternalSource = false;

        private AudioEditor audioEditor;

        private float audioLength;

        bool audio3dMode;
        public bool Loop { get; private set; }

        [SerializeField] private GameObject icon;
        public Sprite iconSprite;
        [SerializeField] private Sprite pauseIcon;

        public SpriteRenderer iconImage;

        public string AudioSpatialType
        {
            get;set;
        }

        public DialogRecorder DialogRecorderPanel
        {
            get; set;
        }


        [HideInInspector]
        public bool isReady = false;
        [HideInInspector]
        public bool isPlaying = false;


        private ToggleObject _obj;


        public ToggleObject GetMyAnnotation()
        {
            return _obj;
        }

        private GameObject _contentObject;

        private void Awake()
        {
            // Makes no sense to use gaze guide with audio...
            UseGuide = false;

            var actionEditor = FindObjectOfType<ActionEditor>();
            audioEditor = (AudioEditor)actionEditor.CreateEditorView(ContentType.AUDIO);
            
        }


        /// <summary>
        /// Initialization method.
        /// </summary>
        /// <param name="obj">Action toggle object.</param>
        /// <returns>Returns true if initialization succesfull.</returns>
        public override bool Init (ToggleObject obj)
        {
            _obj = obj;
            
            // Check that url is not empty.
            if (string.IsNullOrEmpty (obj.url))
            {
                Debug.Log ("Content URL not provided.");
                return false;
            }
            
            // Try to set the parent and if it fails, terminate initialization.
            if (!SetParent (obj))
            {
                Debug.Log ("Couldn't set the parent.");
                return false;
            }

            // Set name.
            name = obj.predicate;

            //check audio is 2d or 3d
            audio3dMode = obj.option.Split('#')[0] == "3d" ? true : false;

            float radius = 0f;


            //3d
            if (audio3dMode)
            {
                //loop is off for 2d and load the status of loop if it is 3d audio
                Loop = false;
                if (audio3dMode)
                    Loop = obj.option.Split('#')[1] == "1";

                //get the radius if it is 3d audio

                if (audio3dMode)
                    radius = float.Parse(obj.option.Split('#')[2]);
            }else
            {
                Destroy(icon);
            }



            // Load audio from resources.
            if (obj.url.StartsWith ("resources://"))
            {
                audioName = obj.url.Replace("resources://", "");

                CreateAudioPlayer (false, audio3dMode, radius, Loop);
            }

            // Load audio from server.
            else
            {
                audioName = obj.url;

                CreateAudioPlayer (true, audio3dMode, radius, Loop);
            }

            // If all went well, return true.
            return true;
        }


        void Update ()
        {

            if (audioEditor && (audioEditor.IsRecording || audioEditor.IsPlaying))
            {
                StopAudio();
                if(iconImage)
                 iconImage.sprite = pauseIcon;
                return;
            }


            if (isPlaying == false)
            {
                if (isReady == true)
                {
                    PlayAudio ();
                }
            }
            if (Input.GetKeyUp (KeyCode.M))
            {
                MuteAudio ();
            }
        }

        /// <summary>
        /// This method starts playback of the audio file, unmuted and on full volume.
        /// If the audio track is already being played, it will be restarted from the beginning. 
        /// If audio source doesn't exist, or hasn't finished loading, the method returns without doing anything
        /// </summary>
        public void PlayAudio()
        {
            if (isReady == false)
            {
                return;
            }
            var audioSource = gameObject.GetComponent<AudioSource>();
          
            if (audioSource != null)
            {
                if (audioSource.isPlaying == true)
                {
                    audioSource.Stop ();
                }
                audioSource.mute = false;
                audioSource.volume = 1.0f;
                audioSource.Play ();
                isPlaying = true;

                audioLength = audioSource.clip.length;
                var myTrigger = ActivityManager.Instance.ActiveAction.triggers.Find(t => t.id == _obj.poi);
                if (myTrigger != null)
                {
                    StartCoroutine(ActivateTrigger(audioSource, myTrigger));
                }
            }
        }

        private static IEnumerator ActivateTrigger(AudioSource audioSource, Trigger trigger)
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }

            if (!ActivityManager.Instance.EditModeActive)
            {
                var triggerDuration = trigger.duration;
                yield return new WaitForSeconds(triggerDuration);

                if (!ActivityManager.Instance.IsLastAction(ActivityManager.Instance.ActiveAction))
                {
                    if (ActivityManager.Instance.ActiveAction != null)
                    {
                        ActivityManager.Instance.ActiveAction.isCompleted = true;
                    }

                    ActivityManager.Instance.ActivateNextAction();
                    TaskStationDetailMenu.Instance.SelectedButton = null;
                }
            }
        }

        /// <summary>
        /// Pause the playback of audio, or if already paused, resume play
        /// </summary>
        public void PauseAudio ()
        {
            if (isReady == false)
            {
                return;
            }
            AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
            if (audioSource != null)
            {
                if (audioSource.isPlaying == true)
                {
                    audioSource.Pause ();
                    isPlaying = false;
                }
                else
                {
                    audioSource.Play ();
                    isPlaying = true;
                }
            }
        }


        /// <summary>
        /// Stop the playback of audio
        /// </summary>
        public void StopAudio ()
        {
            if (isReady == false)
            {
                return;
            }
            AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
            if (audioSource != null)
            {
                if (audioSource.isPlaying == true)
                {
                    audioSource.Stop ();
                    isPlaying = false;
                }
            }
        }


        /// <summary>
        /// Set the volume of the audio
        /// </summary>
        /// <param name="targetVolume">0.0-1.0</param>
        public void SetAudioVolume (float targetVolume)
        {
            if (isReady == false)
            {
                return;
            }
            AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
            if (audioSource != null)
            {
                audioSource.mute = false;
                audioSource.volume = targetVolume;
            }
        }

        /// <summary>
        /// This method mutes the audio, or unmutes it, if it is already muted
        /// </summary>
        public void MuteAudio ()
        {
            if (isReady == false)
            {
                return;
            }
            AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
            if (audioSource != null)
            {
                audioSource.mute = !audioSource.mute;
            }
        }


        /// <summary>
        /// This method is used to set the audio to loop itself or be a one-shot
        /// </summary>
        /// <param name="setLooping">if true, audio repeats itself</param>
        public void LoopAudio (bool setLooping)
        {
            if (isReady == false)
            {
                return;
            }
            AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
            if (audioSource != null)
            {
                audioSource.loop = setLooping;
            }
        }


        /// <summary>
        /// This method creates an audio player
        /// Destroys any already existing audio player in this GameObject.
        /// PlayAudio() must be called to start the audio playback.
        /// </summary>
        /// <param name="useExternalAudioSource">If true, load video from application's LocalState folder, if false, load from project resources.</param>
        public void CreateAudioPlayer (bool useExternalAudioSource = false, bool spatialAudio = false, float radius = 0f,  bool loopAduio = false)
        {

            useExternalSource = useExternalAudioSource;
            if (gameObject.GetComponent<AudioSource> () != null)
            {
                Destroy (gameObject.GetComponent<AudioSource> ());
            }
            isReady = false;
            isPlaying = false;

            // Create audio source component 
            var audioPlayer = gameObject.AddComponent<AudioSource> ();

            if (spatialAudio)
            {
                audioPlayer.spatialBlend = 1;
                audioPlayer.rolloffMode = AudioRolloffMode.Linear;
                audioPlayer.minDistance = 0.1f;
                audioPlayer.maxDistance = radius;
            }

            audioPlayer.loop = loopAduio;

            if (useExternalSource == true)
            {
                StartCoroutine (nameof(LoadAudio));
            }
            else
            {
                // If the audio clip name has a suffix, remove it
                if (audioName.EndsWith (".mp3") || audioName.EndsWith (".wav"))
                {
                    audioName = audioName.Substring (0, audioName.Length - 4);
                }
                Debug.Log ("Trying to load audio: " + audioName);
                AudioClip audioClip = Resources.Load (audioName, typeof (AudioClip)) as AudioClip;
                audioPlayer.clip = audioClip;
                audioPlayer.playOnAwake = false;
                isReady = true;
            }
            //_contentObject = ActionContentStorageManager.Instance.CreateObject(gameObject, _obj);
        }


        private IEnumerator LoadAudio ()
        {
            AudioSource audioPlayer = gameObject.GetComponent<AudioSource> ();
            if (audioName.StartsWith ("http") == false)
            {
                // Local file
                string dataPath = Application.persistentDataPath;
                string completeAudioName = "file://" + dataPath + "/" + audioName;
                Debug.Log ("Trying to load audio: " + completeAudioName);
                WWW www = new WWW (completeAudioName);
                yield return www;
                AudioClip audioClip = www.GetAudioClip (false, false, AudioType.WAV);
                audioPlayer.clip = audioClip;
                audioPlayer.playOnAwake = false;
                isReady = true;
            }

            else
            {
                // Online file stored locally
                var url = audioName.Split('/');
                var filename = url[url.Length - 1];

                var completeAudioName = "file://" + ActivityManager.Instance.Path + "/" + filename;
                Debug.Log("Trying to load audio: " + completeAudioName);
                WWW www = new WWW(completeAudioName);
                yield return www;
                AudioClip audioClip = www.GetAudioClip(false, false, AudioType.WAV);
                audioPlayer.clip = audioClip;
                audioPlayer.playOnAwake = false;
                //audioPlayer.loop = false;
                isReady = true;

                // Online file
                /*
                Debug.Log ("Trying to download audio: " + audioName);
                WWW www = new WWW (audioName);
                yield return www;
                AudioClip audioClip = www.GetAudioClip (false, false, AudioType.WAV);
                audioPlayer.clip = audioClip;
                audioPlayer.playOnAwake = false;
                audioPlayer.loop = false;
                isReady = true;
                */
            }
        }

        private void OnDestroy()
        {
            if (_contentObject != null)
                Destroy(_contentObject);
        }

        public float getAudioLength() {
            return audioLength;
        }


        public float getCurrenttime() {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            return audioSource.time;
        }
    }
}