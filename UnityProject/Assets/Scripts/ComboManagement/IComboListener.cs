using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IComboListener
{
    //Llamado mientras un combo step largo se esta haciendo
    void OnComboStepDoing(ComboStep step, float time);

    //Llamado cuando se cancela un combostep
    void OnComboStepCancelled(ComboStep step);

    //Llamado al iniciarse un step
    void OnComboStepStarted(ComboStep step);

    //Llamado al finalizar un step
    void OnComboStepFinished(ComboStep step);



    //Llamado cuando se ha empezado un combo
    void OnComboStarted(Combo combo);

    //Llamado cuando se ha acabado un combo entero
    void OnComboFinished(Combo combo);

    //Llamado cuando se ha acabado un combo entero
    void OnComboCancelled(Combo combo);
}
