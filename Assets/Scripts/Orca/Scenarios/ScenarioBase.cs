using RVO;
using UnityEngine;

public class ScenarioBase : MonoBehaviour
{
    public GameObject prefab;
    public GameObject prefabAlternative;
    public int agents = 100;
    // goal vectors for each agent
    protected Vector3[] goals;
    // agents in the Unity scene
    protected GameObject[] RVOAgents;

    protected virtual void Start()
    {
        Random.InitState(69);
        Simulator.Instance.setTimeStep(0.05f);
        Simulator.Instance.setAgentDefaults(15.0f, 10, 5.0f, 5.0f, 0.5f, 1.0f, new RVO.Vector2(0.0f, 0.0f));

        RVOAgents = new GameObject[agents];
        goals = new Vector3[agents];
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
