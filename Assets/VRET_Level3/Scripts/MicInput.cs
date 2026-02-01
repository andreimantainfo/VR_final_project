using UnityEngine;

public class MicInput : MonoBehaviour
{
    public float sensitivity = 100f; 
    public float loudness = 0f;
    private AudioClip micClip;
    private string deviceName;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            deviceName = Microphone.devices[0];
            micClip = Microphone.Start(deviceName, true, 10, 44100);
        }
    }

    void Update()
    {
        loudness = GetLoudnessFromMic() * sensitivity;
    }

    float GetLoudnessFromMic()
    {
        if (Microphone.GetPosition(deviceName) > 0)
        {
            float[] waveData = new float[128];
            int micPosition = Microphone.GetPosition(deviceName) - 128 + 1;
            if (micPosition < 0) return 0;
            micClip.GetData(waveData, micPosition);

            float totalLoudness = 0;
            foreach (var sample in waveData) { totalLoudness += Mathf.Abs(sample); }
            return totalLoudness / 128;
        }
        return 0;
    }
}