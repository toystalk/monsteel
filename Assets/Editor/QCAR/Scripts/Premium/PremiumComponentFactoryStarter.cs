/*==============================================================================
Copyright (c) 2013 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Qualcomm Confidential and Proprietary
==============================================================================*/

using System.IO;
using UnityEditor;

/// <summary>
/// Small utility class to create an instance of the VuforiaBehaviourComponentFactory in the editor before anything is initialized.
/// </summary>
[InitializeOnLoad]
public class PremiumComponentFactoryStarter
{
    /// <summary>
    /// register an instance of the VuforiaBehaviourComponentFactory class at the singleton immediately
    /// </summary>
    static PremiumComponentFactoryStarter()
    {
        PremiumBehaviourComponentFactory.Instance = new VuforiaPremiumBehaviourComponentFactory();
    }
}