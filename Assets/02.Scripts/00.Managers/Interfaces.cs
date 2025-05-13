using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

interface IInteract
{
    void Interact();
}
interface IInteractNum
{
    void Interact(int num);
}

interface ExcelReader
{
    void ReadCSV();
    void SettingData();
}

interface IHaveHP
{
    float HP { get; set; }
    float MaxHP { get; set; }

    void GetDamage(float amount);
}