/*==============================================================================
Copyright (c) 2013 QUALCOMM Austria Research Center GmbH.
All Rights Reserved.
Qualcomm Confidential and Proprietary
==============================================================================*/

using UnityEngine;

/// <summary>
/// Small utility behaviour to create an instance of the PremiumVuforiaBehaviourComponentFactory at runtime before anything is initialized.
/// </summary>
public class PremiumComponentFactoryStarterBehaviour : MonoBehaviour
{
    void Awake()
    {
        PremiumBehaviourComponentFactory.Instance = new VuforiaPremiumBehaviourComponentFactory();
    }
}