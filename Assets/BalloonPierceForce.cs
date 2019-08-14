using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPierceForce : MonoBehaviour
{
    public ConstantForce force;

    private bool isFlailing = true;
    public float cogBounceTheta = 0; // we'll modulate the the theta to control the speed of the rotation
    public float cogBounceThetaDegredation;
    public float cogBounceAmplitude = 4;
    public float cogBounceAmplitudeDegredation = -0.5f;
    private float cogBounceAmplitudeInternal;
    private float cogBounceAmplitudeDegredationInternal;
    private float cogBounceThetaInternal;
    private float cogBounceThetaDegredationInternal;
    private float rotateAmount;

    // Start is called before the first frame update
    void Start()
    {
        if( force == null)
        {
            force = gameObject.GetComponent<ConstantForce>();
        }
        force.relativeForce.Set(1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

        force.relativeForce.Set(1.0f, 1.0f, 1.0f);
        if (force != null)
        {

            force.relativeForce.Set(1.0f, 1.0f, 1.0f);
            // handle animating the cogs decoupled from user input
            // animate the cogs flinging back
            if (isFlailing == true)
            {
                // we'll make these cogs sprang back around the end of their rotation point, like there's a bit of elasticity in the mechanism
                // we'll do this by lerping a decaying sine wave
                cogBounceThetaInternal += cogBounceThetaDegredationInternal;
                cogBounceAmplitudeInternal += cogBounceAmplitudeDegredationInternal;

                if (cogBounceAmplitudeInternal > 0)
                    rotateAmount = cogBounceAmplitudeInternal * Mathf.Sin(cogBounceThetaInternal);

                if (cogBounceAmplitudeInternal <= 0)
                {
                    // cancel cog animations
                    isFlailing = false;
                    cogBounceThetaInternal = cogBounceTheta;
                    cogBounceThetaDegredationInternal = cogBounceThetaDegredation;
                    cogBounceAmplitudeDegredationInternal = cogBounceAmplitudeDegredation;
                    cogBounceAmplitudeInternal = cogBounceAmplitude;
                }

                force.relativeForce.Set(1.0f, 1.0f, 1.0f);

            }
        }
    }
}
