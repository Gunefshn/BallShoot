using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameManager _GameManager;
    Rigidbody rb;
    Renderer color;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        color = GetComponent<Renderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bucket"))
        {
            TechnicalProcess();
            _GameManager.BallEntered();
        }
        else if (other.CompareTag("SubObject"))
        {
            TechnicalProcess();
            _GameManager.BallNotEntered(); 
        }
    }
    void TechnicalProcess()
    {
        _GameManager.ParcEffect(gameObject.transform.position, color.material.color);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);

    }
}
