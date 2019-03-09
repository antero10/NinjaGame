using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour, IAnimationCompleted
{
    public MenuClassifier menuClassifier;

    public enum StartMenuStates
    {
        Ignore,
        Active,
        Disabled
    };
    public StartMenuStates startingState = StartMenuStates.Active;
    public bool resetPositionOnStart = true;

    public UnityEvent onRefreshMenu = new UnityEvent();

    private Animator animator;
    private bool openState = false;

    public bool IsOpen
    {
        get
        {
            if (animator != null)
            {
                return animator.GetBool("IsOpen");
            }
            return openState;
        }

        set
        {
            if (value == true)
            {
                gameObject.SetActive(true);
                onRefreshMenu.Invoke();
            }

            if (animator != null)
            {
                animator.SetBool("IsOpen", value);
            }
            else
            {
                gameObject.SetActive(false);
            }
            openState = value;
        }
    }

    public void RefreshMenu()
    {
        onRefreshMenu.Invoke();
    }

    public virtual void onShowMenu()
    {
        IsOpen = true;
    }

    public virtual void onHideMenu()
    {
        IsOpen = false;
    }

    public virtual void Awake()
    {
        if (resetPositionOnStart == true)
        {
            var rect = GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
        }
    }

    public virtual void Start()
    {
        animator = GetComponent<Animator>();

        // TODO: Register with Menu Manager
        MenuManager.Instance.addMenu(this);

        switch(startingState)
        {
            case StartMenuStates.Active:
                gameObject.SetActive(true);
                break;

            case StartMenuStates.Disabled:
                gameObject.SetActive(false);
                break;
        }

        openState = gameObject.activeInHierarchy;
    }

    public void AnimationCompleted(int shortHashName)
    {
        if (IsOpen == false)
        {
            gameObject.SetActive(false);
        }
    }
}
