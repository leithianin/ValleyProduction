using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisitorBehavior : VLY_Component
{
    [SerializeField] private CPN_Movement movement;
    [SerializeField] private VLY_ComponentHandler handler;

    public VLY_ComponentHandler Handler => handler;

    private PathFragmentData currentPathFragment = null;

    public VisitorScriptable visitorType;

    [SerializeField] private UnityEvent<VisitorScriptable> OnSetVisitorWithType;
    [SerializeField] private UnityEvent<VisitorScriptable> OnUnsetVisitorWithType;
    [SerializeField] private UnityEvent<VLY_Component> OnAddComponent;
    [SerializeField] private UnityEvent<VLY_Component> OnRemoveComponent;

    private AnimationHandler visitorDisplay = null;

    List<Vector3> interuptedPath = new List<Vector3>();

    private bool isUsed = false;

    private List<PathFragmentData> walkedPathFragment = new List<PathFragmentData>();

    [SerializeField] private CPN_IsLandmark currentObjective;
    private bool isGoingHome;

    public bool IsUsed => isUsed;

    public CPN_Movement Movement => movement;

    private void Start()
    {
        movement.PlayOnEndWalking.AddListener(ReachDestination);
    }

    /// <summary>
    /// Active le visiteur.
    /// </summary>
    /// <param name="nSpawnPoint">Le point de spawn du visiteur.</param>
    /// <param name="spawnPosition">La position de spawn du visiteur.</param>
    /// <param name="nVisitorType">Le type de visiteur voulut.</param>
    /// <param name="nPath">Le chemin choisit par le visiteur.</param>
    public void SetVisitor(IST_PathPoint nSpawnPoint, Vector3 spawnPosition, VisitorScriptable nVisitorType, CPN_IsLandmark objective)
    {
        walkedPathFragment = new List<PathFragmentData>();

        currentObjective = objective;

        visitorType = nVisitorType;

        isGoingHome = false;

        currentPathFragment = SearchFirstPathFragment(nSpawnPoint);

        if(currentPathFragment != null)
        {
            OnSetVisitorWithType?.Invoke(visitorType);

            walkedPathFragment.Add(currentPathFragment);

            movement.SetSpeed(UnityEngine.Random.Range(visitorType.Speed.x, visitorType.Speed.y));

            transform.position = spawnPosition;

            gameObject.SetActive(true);

            CPN_Informations information = handler.GetComponentOfType<CPN_Informations>();
            if(information != null)
            {
                information.visitorType = visitorType.Type;
            }

            if(visitorDisplay != null)
            {
                OnRemoveComponent?.Invoke(visitorDisplay);

                Destroy(visitorDisplay.gameObject);
            }

            isUsed = true;

            visitorDisplay = Instantiate(visitorType.Display, transform);

            OnAddComponent?.Invoke(visitorDisplay);

            movement.WalkOnNewPath(currentPathFragment.path);
        }
    }

    private void DeleteVisitor(IST_PathPoint pathpoint)
    {
        VisitorManager.DeleteVisitor(this);
    }

    /// <summary>
    /// Désactive le visiteur.
    /// </summary>
    public void UnsetVisitor()
    {
        isUsed = false;
        OnUnsetVisitorWithType?.Invoke(visitorType);
        currentPathFragment = null;

        movement.InteruptWalk();

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Cherche le prochain Pathpoint à atteindre.
    /// </summary>
    public void SearchDestination()
    {
        if (currentPathFragment != null)
        {
            currentPathFragment.endPoint.OnDestroyPathPoint -= DeleteVisitor;
        }

        currentPathFragment = SearchNextPathFragment();

        if (currentPathFragment == null)
        {
            DeleteVisitor(null);
        }
        else
        {
            walkedPathFragment.Add(currentPathFragment);

            currentPathFragment.endPoint.OnDestroyPathPoint += DeleteVisitor;

            movement.WalkOnNewPath(currentPathFragment.path);
        }
    }

    /// <summary>
    /// Appelé quand le visiteur atteint un PathPoint.
    /// </summary>
    public void ReachDestination()
    {
        // Check si despawn ou autre

        if (currentPathFragment == null || currentPathFragment.endPoint == null)
        {
            VisitorManager.DeleteVisitor(this);
        }
        else
        {
            SearchDestination();
        }
    }

    /// <summary>
    /// Interromp le déplacement du visiteur.
    /// </summary>
    public void InteruptWalk()
    {
        movement.PlayOnEndWalking.RemoveListener(ReachDestination);
        interuptedPath = new List<Vector3>(movement.InteruptWalk());
    }

    /// <summary>
    /// Reprend le déplacement du visiteur où il s'était arrété sur le chemin.
    /// </summary>
    public void ContinueWalk()
    {
        movement.PlayOnEndWalking.AddListener(ReachDestination);
        movement.WalkOnNewPath(interuptedPath);
    }

    /// <summary>
    /// Cherche le premier PathFragment à parcourir.
    /// </summary>
    /// <param name="startPoint">Le point de départ du PathFragment recherché.</param>
    /// <returns>Le PathFragment à parcourir.</returns>
    private PathFragmentData SearchFirstPathFragment(IST_PathPoint startPoint)
    {
        PathFragmentData pathToTake = startPoint.Node.GetMostInterestingPath(currentObjective, null, visitorType.LikedInteractions(), visitorType.HatedInteractions(), walkedPathFragment);

        if (pathToTake != null && pathToTake.endPoint == startPoint)
        {
            pathToTake = new PathFragmentData(pathToTake.endPoint, pathToTake.startPoint, pathToTake.GetReversePath(), false);
        }

        return pathToTake;
    }

    /// <summary>
    /// Cherche le prochain PathFragment à parcourir.
    /// </summary>
    /// <returns>Le PathFragment à parcourir.</returns>
    private PathFragmentData SearchNextPathFragment()
    {
        NodePathData nodeData = currentPathFragment.endPoint.Node.GetDataForLandmarkType(currentObjective);

        if (nodeData != null && (nodeData.linkedToLandmark || nodeData.distanceFromLandmark < 0))
        {
            if (currentObjective.Type == LandmarkType.Spawn)
            {
                return null;
            }
            else
            {
                List<CPN_IsLandmark> spawns = VLY_LandmarkManager.GetLandmarkOfType(LandmarkType.Spawn);
                currentObjective = spawns[UnityEngine.Random.Range(0, spawns.Count)];
                isGoingHome = true;
            }
        }
        else if(currentObjective == null)
        {
            List<CPN_IsLandmark> spawns = VLY_LandmarkManager.GetLandmarkOfType(LandmarkType.Spawn);
            currentObjective = spawns[UnityEngine.Random.Range(0, spawns.Count)];
        }

        List<SatisfactorType> likedType = new List<SatisfactorType>();
        List<SatisfactorType> hatedType = new List<SatisfactorType>();

        if(!isGoingHome)
        {
            likedType = visitorType.LikedInteractions();
            hatedType = visitorType.HatedInteractions();
        }

        PathFragmentData pathToTake = currentPathFragment.endPoint.Node.GetMostInterestingPath(currentObjective, currentPathFragment, likedType, hatedType, walkedPathFragment);

        if(pathToTake != null && pathToTake.endPoint == currentPathFragment.endPoint)
        {
            pathToTake = new PathFragmentData(pathToTake.endPoint, pathToTake.startPoint, pathToTake.GetReversePath(), false);
        }

        return pathToTake;
    }

    /// <summary>
    /// Joue l'animation de mouvement.
    /// </summary>
    public void PlayMovementAnimation()
    {
        visitorDisplay.PlayBodyAnim(BodyAnimationType.Walk);
    }

    /// <summary>
    /// Stop l'animation de mouvement.
    /// </summary>
    public void StopMovementAnimation()
    {
        visitorDisplay.StopBodyAnim(BodyAnimationType.Walk);
    }
}
