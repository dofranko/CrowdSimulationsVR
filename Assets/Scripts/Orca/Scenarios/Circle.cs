using RVO;
using UnityEngine;

public class Circle : ScenarioBase
{
    public float ringSize = 100;

    protected override void Start()
    {
        base.Start();

        Vector3 facingOrigin = new(0, 0, 0);
        int agentToDislocateIndex = Random.Range(0, agents - 1);

        for (int i = 0; i < agents; i++)
        {
            float angle = (float)i / agents * (float)System.Math.PI * 2;
            Vector3 pos = new Vector3((float)System.Math.Cos(angle), 0, (float)System.Math.Sin(angle)) * ringSize;
            Vector3 antipodal = -pos;

            GameObject go = Instantiate(prefabBlue, Vector3.zero, Quaternion.Euler(0, angle + 180, 0));

            go.transform.parent = transform;
            go.transform.position = pos;
            if (i == agentToDislocateIndex)
            {
                pos.x += 3.0f;
                go.transform.position = pos;
            }

            Vector3 dir = (facingOrigin - go.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            go.transform.rotation = Quaternion.Slerp(go.transform.rotation, lookRotation, 1);

            goals[i] = antipodal;
            RVOAgents[i] = go;

            Simulator.Instance.addAgent(Rvo2Helper.ToRVOVector(go.transform.position));
        }
    }


}
