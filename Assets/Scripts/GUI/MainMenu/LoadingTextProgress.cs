using UnityEngine;
using TMPro;

public class LoadingTextProgress : MonoBehaviour
{
    public TextMeshProUGUI text;

    public float interpolationSpeed = 1.0f;

    private void Update()
    {
        if (LoadingProgress.isLoading)
        {
            AlhpaInterpolate();
        }
    }

    private void AlhpaInterpolate()
    {
        float alpha = Mathf.PingPong(Time.time * interpolationSpeed, 1.0f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
}