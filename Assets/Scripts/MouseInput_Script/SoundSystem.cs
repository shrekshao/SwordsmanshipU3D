using UnityEngine;
using System.Collections;

namespace Swordsmanship {

    enum SOUND_ID {
        SOUND_FIRE, SOUND_SWING, SOUND_THUNDER, SOUND_TORNADO
    };

    public class SoundSystem : MonoBehaviour {

        private const int nClips = 4;
        private AudioClip[] clips;
        private AudioSource audioSource;

        public void Start() {
            audioSource = new AudioSource();
            clips = new AudioClip[ nClips ];
            clips[ ( int )SOUND_ID.SOUND_FIRE ] = Resources.Load< AudioClip >( "sound/fire" );
            clips[ ( int )SOUND_ID.SOUND_SWING ] = Resources.Load< AudioClip >( "sound/swing" );
            clips[ ( int )SOUND_ID.SOUND_THUNDER ] = Resources.Load< AudioClip >( "sound/thunder" );
            clips[ ( int )SOUND_ID.SOUND_TORNADO ] = Resources.Load< AudioClip >( "sound/tornado" );
        }

        public void play( int id, GameObject playObject ) {
            AudioSource.PlayClipAtPoint( clips[ id ], playObject.transform.position );
        }
    }
}