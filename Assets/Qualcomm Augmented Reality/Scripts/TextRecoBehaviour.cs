/*==============================================================================
Copyright (c) 2013 QUALCOMM Austria Research Center GmbH.
All Rights Reserved.
Confidential and Proprietary - QUALCOMM Austria Research Center GmbH.
==============================================================================*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// This is the main behaviour class that encapsulates text recognition behaviour.
/// It just has to be added to a Vuforia-enabled Unity scene and will initialize the text tracker with the configured word list.
/// Events for newly recognized or lost words will be called on registered ITextRecoEventHandlers
/// </summary> 
public class TextRecoBehaviour : TextRecoAbstractBehaviour
{
}