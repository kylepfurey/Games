using UnityEngine;

public class ItemShellHit : MonoBehaviour
{
    public ItemShell Shell;

    // Rotate Shell
    void FixedUpdate()
    {
        if (Shell.rotate)
        {
            transform.eulerAngles += new Vector3(0, 7.5f, 0);
        }
    }

    // Entering Collision
    public void OnCollisionEnter(Collision collision)
    {
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 contactNormal = collision.contacts[0].normal;

        if (Shell.placed)
        {
            // Bumping Wall or Bumper
            if ((collision.collider.tag == "Wall" || collision.collider.tag == "Bumper") && Shell.bounceCount == 0)
            {
                Shell.Kart1.Audio.Play("Shell Break");

                Destroy(transform.parent.gameObject);
            }
            else if (collision.collider.tag == "Wall" || collision.collider.tag == "Bumper")
            {
                Shell.bounceCount -= 1;

                // Bounce Angles
                Vector3 orignalRotation = Shell.transform.eulerAngles;
                Shell.transform.LookAt(contactPoint);

                if (contactNormal.x > 0.5f || contactNormal.x < -0.5f)
                {
                    Shell.transform.eulerAngles = new Vector3(orignalRotation.x, -Shell.transform.eulerAngles.y + 180, orignalRotation.z);
                }
                else
                {
                    Shell.transform.eulerAngles = new Vector3(orignalRotation.x, -Shell.transform.eulerAngles.y, orignalRotation.z);
                }

                Shell.Kart1.Audio.Play("Shell Bump");
            }
        }
    }
    // In Collision
    public void OnCollisionStay(Collision collision)
    {
        // Touching Ground
        if (collision.collider.tag == "Ground")
        {
            Shell.shellSpeed = 1.5f;
        }
        else
        {
            Shell.shellSpeed = 0.75f;
        }
    }

    // Collision
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (Shell.placed)
        {
            // Bumping Obstacle
            if (other.tag == "Obstacle")
            {
                Destroy(other.transform.parent.gameObject);
                Destroy(transform.parent.gameObject);
            }

            // Hitting Bouncer
            if (other.tag == "Bouncer")
            {
                Shell.Rigidbody.AddForce(new Vector3(0, 12, 0), ForceMode.Impulse);
            }
        }
    }
}