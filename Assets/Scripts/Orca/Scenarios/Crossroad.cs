using RVO;
using UnityEngine;

public class Crossroad : ScenarioBase
{
    public float pathLength = 100;
    protected override void Start()
    {
        base.Start();

        Vector3 facingOrigin = new(0, 0, 0);

        for (int i = 0; i < agents; i++)
        {
            int side = i % 4;
            Vector3 pos = new(Random.Range(-scatterRange.x, scatterRange.x), 0, Random.Range(-scatterRange.z, scatterRange.z));
            Vector3 antipodal = new(0, 0, 0);
            GameObject selectedPrefab = prefabBlue;
            if (side == 0)
            {
                pos += new Vector3(pathLength / 2, 0, 0);
                antipodal += new Vector3(-pathLength / 2, 0, 0);
                selectedPrefab = prefabBlue;
            }
            if (side == 1)
            {
                pos += new Vector3(-pathLength / 2, 0, 0);
                antipodal += new Vector3(pathLength / 2, 0, 0);
                selectedPrefab = prefabRed;
            }
            if (side == 2)
            {
                pos += new Vector3(0, 0, pathLength / 2);
                antipodal += new Vector3(0, 0, -pathLength / 2);
                selectedPrefab = prefabGreen;
            }
            if (side == 3)
            {
                pos += new Vector3(0, 0, -pathLength / 2);
                antipodal += new Vector3(0, 0, pathLength / 2);
                selectedPrefab = prefabYellow;
            }

            GameObject go = Instantiate(selectedPrefab, Vector3.zero, Quaternion.LookRotation(antipodal));

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
