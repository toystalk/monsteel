/*==============================================================================
Copyright (c) 2012-2013 Qualcomm Austria Research Center GmbH.
All Rights Reserved.
Confidential and Proprietary - QUALCOMM Austria Research Center GmbH.
==============================================================================*/

using UnityEngine;

/// <summary>
/// This Behaviour allows to set a layer ID that is applied to all child gameobjects
/// and the camera is set to cull away everything except this layer.
/// This is necessary because unity packages won't export the layers set up in a project
/// </summary>
[RequireComponent(typeof(Camera))]
public class SetBGCameraLayerBehaviour : SetBGCameraLayerAbstractBehaviour
{
}