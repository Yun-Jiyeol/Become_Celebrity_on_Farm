using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

interface IInteract
{
    void Interact();
}

interface ExcelReader
{
    void ReadCSV();
    void SettingData();
}