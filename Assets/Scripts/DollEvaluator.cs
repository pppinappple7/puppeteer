using UnityEngine;

public static class DollEvaluator
{
    public static DollEvaluationResult Evaluate(
        GameObject doll,
        Customer.ColorRequirement[] colorRequirements,
        Customer.MeshRequirement[] meshRequirements,
        float requiredPercent)
    {
        if (doll == null)
            return DollEvaluationResult.Failure("The doll reference is missing.");

        if (float.IsNaN(requiredPercent) || float.IsInfinity(requiredPercent) ||
            requiredPercent < 0f || requiredPercent > 1f)
        {
            return DollEvaluationResult.Failure("The required percentage must be between 0 and 1.");
        }

        int colorCount = colorRequirements == null ? 0 : colorRequirements.Length;
        int meshCount = meshRequirements == null ? 0 : meshRequirements.Length;
        int totalRequirements = colorCount + meshCount;

        if (totalRequirements == 0)
            return DollEvaluationResult.Failure("The customer has no evaluation requirements.");

        int matchedRequirements = 0;

        if (colorRequirements != null)
        {
            foreach (Customer.ColorRequirement requirement in colorRequirements)
            {
                if (requirement == null || string.IsNullOrWhiteSpace(requirement.objName))
                    return DollEvaluationResult.Failure("A color requirement has no target object name.");

                Transform target = doll.transform.Find(requirement.objName);
                if (target == null)
                    return DollEvaluationResult.Failure(
                        "The required object was not found: " + requirement.objName);

                MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                    return DollEvaluationResult.Failure(
                        "The required object has no MeshRenderer: " + requirement.objName);

                if (meshRenderer.material.color == requirement.color)
                    matchedRequirements++;
            }
        }

        if (meshRequirements != null)
        {
            foreach (Customer.MeshRequirement requirement in meshRequirements)
            {
                if (requirement == null || string.IsNullOrWhiteSpace(requirement.objName))
                    return DollEvaluationResult.Failure("A mesh requirement has no target object name.");

                Transform target = doll.transform.Find(requirement.objName);
                if (target == null)
                    return DollEvaluationResult.Failure(
                        "The required object was not found: " + requirement.objName);

                MeshFilter meshFilter = target.GetComponent<MeshFilter>();
                if (meshFilter == null)
                    return DollEvaluationResult.Failure(
                        "The required object has no MeshFilter: " + requirement.objName);

                if (meshFilter.mesh == requirement.mesh)
                    matchedRequirements++;
            }
        }

        long payment = 0;

        PartPicker[] partPickers = doll.GetComponentsInChildren<PartPicker>();
        foreach (PartPicker partPicker in partPickers)
        {
            if (partPicker.price < 0)
                return DollEvaluationResult.Failure("A part has a negative price.");

            payment += (int)(partPicker.price * 0.5f);
            if (payment > int.MaxValue)
                return DollEvaluationResult.Failure("The calculated payment is too large.");
        }

        Pricer[] pricers = doll.GetComponentsInChildren<Pricer>();
        foreach (Pricer pricer in pricers)
        {
            if (pricer.price < 0)
                return DollEvaluationResult.Failure("A clothing item has a negative price.");

            payment += (int)(pricer.price * 0.5f);
            if (payment > int.MaxValue)
                return DollEvaluationResult.Failure("The calculated payment is too large.");
        }

        float matchPercent = (float)matchedRequirements / totalRequirements;
        bool isGood = matchPercent >= requiredPercent;

        return DollEvaluationResult.Success(
            isGood,
            (int)payment,
            matchedRequirements,
            totalRequirements,
            matchPercent);
    }
}
