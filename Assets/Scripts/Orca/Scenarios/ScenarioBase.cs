using RVO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScenarioBase : MonoBehaviour
{
    public GameObject prefabBlue;
    public GameObject prefabRed;
    public GameObject prefabGreen;
    public GameObject prefabYellow;
    public int agents = 100;
    // goal vectors for each agent
    protected Vector3[] goals;
    // agents in the Unity scene
    protected GameObject[] RVOAgents;
    public Vector3 scatterRange = new(10, 0, 10);
    protected virtual void Start()
    {
        Random.InitState(69);
        Simulator.Instance.setTimeStep(0.05f);
        Simulator.Instance.setAgentDefaults(15.0f, 10, 5.0f, 5.0f, 0.5f, 5.0f, new RVO.Vector2(0.0f, 0.0f));
        AddObstacles();

        RVOAgents = new GameObject[agents];
        goals = new Vector3[agents];
    }

    private void AddObstacles()
    {

        BoxCollider[] boxColliders = FindObjectsOfType<BoxCollider>();
        for (int i = 0; i < boxColliders.Length; i++)
        {
            float minX = boxColliders[i].transform.position.x -
                         boxColliders[i].size.x * boxColliders[i].transform.lossyScale.x * 0.5f;
            float minZ = boxColliders[i].transform.position.z -
                         boxColliders[i].size.z * boxColliders[i].transform.lossyScale.z * 0.5f;
            float maxX = boxColliders[i].transform.position.x +
                         boxColliders[i].size.x * boxColliders[i].transform.lossyScale.x * 0.5f;
            float maxZ = boxColliders[i].transform.position.z +
                         boxColliders[i].size.z * boxColliders[i].transform.lossyScale.z * 0.5f;

            IList<RVO.Vector2> obstacle = new List<RVO.Vector2>
            {
                new RVO.Vector2(maxX, maxZ),
                new RVO.Vector2(minX, maxZ),
                new RVO.Vector2(minX, minZ),
                new RVO.Vector2(maxX, minZ)
            };
            Simulator.Instance.addObstacle(obstacle);
        }

        Simulator.Instance.processObstacles();
    }

    protected virtual void Update()
    {

        for (int i = 0; i < Simulator.Instance.getNumAgents(); i++)
        {
            RVO.Vector2 agentLoc = Simulator.Instance.getAgentPosition(i);
            RVO.Vector2 goalVector = Rvo2Helper.ToRVOVector(goals[i]) - agentLoc;

            if (RVOMath.absSq(goalVector) > 1.0f)
            {
                goalVector = RVOMath.normalize(goalVector);
            }

            Simulator.Instance.setAgentPrefVelocity(i, goalVector);

            RVOAgents[i].transform.localPosition = Rvo2Helper.ToUnityVector(agentLoc);
        }
        Simulator.Instance.doStep();
    }
}
