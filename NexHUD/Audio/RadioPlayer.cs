using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHUD.Audio
{
    public class RadioInfos
    {
        public string name;
        public string url;
    }
    public class RadioPlayer : IDisposable
    {
        #region singleton
        public static RadioPlayer Instance { get { return Nested.instance; } }


        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly RadioPlayer instance = new RadioPlayer();
        }
        #endregion


        private List<RadioInfos> m_radios;
        private int m_currentRadioId = 0;
        private float m_volume = 0.5f;
        private MediaFoundationReader m_mf;
        private WaveOutEvent m_wo;

        public bool isPlaying
        {
            get
            {
                if (m_wo != null)
                    return m_wo.PlaybackState == PlaybackState.Playing;
                else
                    return false;
            }
        }
        public float Volume
        {
            get
            {
                if (m_wo != null)
                    return m_wo.Volume;
                else
                    return m_volume;
            }
            set
            {
                if (m_wo != null)
                    m_wo.Volume = value;
                m_volume = value;
            }
        }
        
        private RadioPlayer()
        {
            registerRadios();
            resetWaveOut();
        }
        private void registerRadios()
        {
            m_radios = new List<RadioInfos>();
            m_radios.Add(new RadioInfos() { name = "Radio sidewinder", url = "http://radiosidewinder.out.airtime.pro:8000/radiosidewinder_b" });
            m_radios.Add(new RadioInfos() { name = "Lave radio", url = "http://bluford.torontocast.com:8421/stream" });
            m_radios.Add(new RadioInfos() { name = "Hutton orbital", url = "http://bluford.torontocast.com:8447/hq" });
        }

        public RadioInfos getRadioInfos()
        {
            return m_radios[m_currentRadioId];
        }
        private void resetWaveOut()
        {
            if (m_mf != null)
                m_mf.Dispose();
            m_mf = new MediaFoundationReader(getRadioInfos().url);
            if (m_wo != null)
                m_wo.Dispose();
            m_wo = new WaveOutEvent();

            m_wo.Init(m_mf);
            m_wo.Volume = m_volume;

        }
        public void Play()
        {
            if (m_wo == null)
                resetWaveOut();
            if (m_wo != null)
                m_wo.Play();
        }
        public void Pause()
        {
            if (m_wo != null)
                m_wo.Pause();
        }
        public void Next()
        {
            if (m_currentRadioId < m_radios.Count - 1)
                m_currentRadioId++;
            else
                m_currentRadioId = 0;

            bool _autoPlay = isPlaying;
            resetWaveOut();
            if (_autoPlay)
                Play();
        }
        public void Prev()
        {
            if (m_currentRadioId > 0)
                m_currentRadioId--;
            else
                m_currentRadioId = m_radios.Count - 1;

            bool _autoPlay = isPlaying;
            resetWaveOut();
            if (_autoPlay)
                Play();
        }
        public void Dispose()
        {
            m_mf.Dispose();
            m_wo.Dispose();
        }
    }
}
