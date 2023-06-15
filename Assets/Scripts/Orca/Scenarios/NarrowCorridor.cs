using RVO;
using UnityEngine;

public class NarrowCorridor : ScenarioBase
{
    public float pathLength = 100;
    protected override void Start()
    {
        base.Start();

        Vector3 facingOrigin = new(0, 0, 0);

        for (int i = 0; i < agents; i++)
        {
            int side = i % 2 == 0 ? 1 : -1;
            Vector3 pos = new Vector3(side * pathLength / 2, 0, 0) + new Vector3(Random.Range(-scatterRange.x, scatterRange.x), 0, Random.Range(-scatterRange.z, scatterRange.z));
            Vector3 antipodal = new(-side * pathLength / 2, 0, 0);

            GameObject go = Instantiate(i % 2 == 0 ? prefabBlue : prefabRed, Vector3.zero, Quaternion.LookRotation(antipodal));

            go.transform.parent = transform;
            go.transform.position = pos;

            Vector3 dir = (facingOrigin - go.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            go.transform.rotation = Quaternion.Slerp(go.transform.rotation, lookRotation, 1);

            goals[i] = antipodal;
            RVOAgents[i] = go;

            Simulator.Instance.addAgent(Rvo2Helper.ToRVOVector(go.transform.position));
        }

    }
}
