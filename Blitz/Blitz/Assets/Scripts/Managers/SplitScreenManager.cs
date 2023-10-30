using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SplitScreenManager : MonoBehaviour
    {
        private List<PlayerInput> players = new List<PlayerInput>();

        [SerializeField]
        private List<LayerMask> playerLayers;

        private PlayerInputManager inputManager;

        private void Awake()
        {
            inputManager = FindObjectOfType<PlayerInputManager>();
            inputManager.onPlayerJoined += AddPlayer;
        }

        public void AddPlayer(PlayerInput player)
        {
            players.Add(player);
 
            int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);

            player.transform.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
            //add the layer
            player.transform.GetChild(0).GetChild(0).GetComponent<Camera>().cullingMask |= 1 << layerToAdd;

            player.transform.GetComponentInChildren<PlayerCamInput>().lookValue = player.actions.FindAction("Look");

        }
    }
