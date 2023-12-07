using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VignetteController : MonoBehaviour
{
    // A reference to the TunnelingVignetteController component
    public TunnelingVignetteController tunnelingVignetteController;

    // A public variable to control the vignette size
    [Range(0f, 1f)]
    public float vignetteSize;

    // Update is called once per frame
    void Update()
    {
        // Set the vignette size to the public variable value
        //tunnelingVignetteController.vignetteSize = vignetteSize;
    }
}