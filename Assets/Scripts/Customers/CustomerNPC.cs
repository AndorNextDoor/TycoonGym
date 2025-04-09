using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomerNPC : MonoBehaviour
{
    private NavMeshAgent agent;
    private CustomerManager.GymItem currentItem;

    public float workoutTime = 30f;

    private Transform exitPoint;
    private Transform cashRegistenPoint;
    private bool IsWaitingForGivingMoney;
    [SerializeField] private Button button;

    private enum State { WalkingToItem, Training, GoingToRegister, WaitingForPayment, Leaving }
    private State state;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        TryUseGymItem();
        exitPoint = CustomerManager.Instance.GetExitPoint();
        cashRegistenPoint = CustomerManager.Instance.GetCashRegisterPoint();
        button.onClick.AddListener(() => AfterPaying());

    }

    void TryUseGymItem()
    {
        currentItem = CustomerManager.Instance.GetAvailableItem();
        if (currentItem != null)
        {
            agent.SetDestination(currentItem.itemTransform.position);
            state = State.WalkingToItem;
        }
        else
        {
            Debug.Log("No available gym item. Leaving.");
            Leave();
        }
    }

    void Update()
    {
        switch (state)
        {
            case State.WalkingToItem:
                if (ReachedDestination())
                {
                    StartCoroutine(WorkoutRoutine());
                    state = State.Training;
                }
                break;
            case State.GoingToRegister:
                if (ReachedDestination())
                {
                    // Simulate payment or call register logic here
                    if(ProgressionManager.Instance.GetCurrentLevel() > 0)
                    {
                        state = State.Leaving;
                        agent.SetDestination(exitPoint.position);
                    }
                    else
                    {
                        state = State.WaitingForPayment;
                        agent.enabled = false;
                        button.gameObject.SetActive(true);
                    }
                }
                break;
            case State.WaitingForPayment:
                break;
        }
    }

    private void AfterPaying()
    {
        agent.enabled = true;
        state = State.Leaving;
        agent.SetDestination(exitPoint.position);
    }

    IEnumerator WorkoutRoutine()
    {
        yield return new WaitForSeconds(workoutTime);
        CustomerManager.Instance.ReleaseItem(currentItem);
        GoToRegister();
    }

    void GoToRegister()
    {
        agent.SetDestination(cashRegistenPoint.position);
        state = State.GoingToRegister;
    }

    void Leave()
    {
        agent.SetDestination(exitPoint.position);
        Destroy(gameObject, 5f); // remove after leaving
    }
    bool ReachedDestination() => !agent.pathPending && agent.remainingDistance < 0.5f;
}
