using UnityEngine;

public class BoidsAgent : MonoBehaviour
{
    private float speed;
    private BoidsManager bm;
    Rigidbody rb;
    private bool isGoalReached = false;

    void Start()
    {
        Random.InitState(2137);
    }

    void Update()
    {
        if (rb) rb.velocity /= 10f;
        if (isGoalReached)
            return;

        if (Random.Range(0, 100) < 10)
        {
            ApplyRules();
        }

        transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, bm.goalPos) < bm.goalRadious)
            isGoalReached = true;
    }

    public void Initialize(BoidsManager bm)
    {
        this.bm = bm;
        speed = Random.Range(bm.minSpeed, bm.maxSpeed);
        transform.LookAt(bm.goal.transform);
        rb = GetComponent<Rigidbody>();
    }

    private void ApplyRules()
    {
        GameObject[] gos = bm.allBoids;
        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.1f;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {

            if (go == gameObject)
                continue;

            float mDistance = Vector3.Distance(go.transform.position, transform.position);
            if (mDistance <= bm.neighbourDistance)
            {
                vCentre += go.transform.position;
                groupSize++;
                gSpeed += go.GetComponent<BoidsAgent>().speed;

                if (mDistance < bm.avoideDistance)
                    vAvoid += transform.position - go.transform.position;
            }

        }

        if (groupSize > 0)
        {
            vCentre = vCentre / groupSize + (bm.goalPos - transform.position);
            speed = gSpeed / groupSize;

            if (speed > bm.maxSpeed)
                speed = bm.maxSpeed;


            Vector3 direction = vCentre + vAvoid - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                                                    transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    bm.rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(
                                                transform.rotation,
                                                Quaternion.LookRotation(bm.goalPos - transform.position),
                                                bm.rotationSpeed * Time.deltaTime);
        }
    }

}
