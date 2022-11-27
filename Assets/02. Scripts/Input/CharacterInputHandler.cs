using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{

    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    bool isFireButtonPressed = false;


    LocalCameraHandler localCameraHandler;
    CharacterMovementHandler characterMovementHandler;
    // Start is called before the first frame update
    void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponent<CharacterMovementHandler>();

    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!characterMovementHandler.Object.HasInputAuthority)
            return;


        // view input
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1;

        // move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");


        // Jump input
        if (Input.GetButtonDown("Jump"))
            isJumpButtonPressed = true;

        if (Input.GetButtonDown("Fire1"))
            isFireButtonPressed = true;

        // set view
        localCameraHandler.SetViewInputVector(viewInputVector);
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        // view Data
        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        // move data
        networkInputData.movementInput = moveInputVector;

        // jump data
        networkInputData.isJumpPressed = isJumpButtonPressed;

        // fire data
        networkInputData.isFirePressed = isFireButtonPressed;

        // reset variables now that we have read their states
        isJumpButtonPressed = false;
        isFireButtonPressed = false;

        return networkInputData;
    }
}
