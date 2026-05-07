using Photon.Pun;
using UnityEngine;

public class PlayerVisual : MonoBehaviourPun
{
    public Renderer playerRenderer;

    public Color p1Color = Color.yellow;
    public Color p2Color = Color.red;

    void Start()
    {
        object role;

        if (photonView.Owner.CustomProperties.TryGetValue("PlayerRole", out role))
        {
            string playerRole = role.ToString();

            if (playerRole == "P1")
            {
                playerRenderer.material.color = p1Color;
            }
            else if (playerRole == "P2")
            {
                playerRenderer.material.color = p2Color;
            }
        }
    }
}