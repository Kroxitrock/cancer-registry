using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CancerRegistry.Models.Diagnoses;
using CancerRegistry.Models.Diagnoses.Treatments;

namespace CancerRegistry.Services
{
    public class StateTranslator
    {
        public static string TranslateTumorState(PrimaryTumorState state)
        {
            var primaryTumorState = "";
            if (state == PrimaryTumorState.T0) primaryTumorState += "T0";
            else if (state == PrimaryTumorState.T1) primaryTumorState += "T1 = tumor size ≤20 mm";
            else if (state == PrimaryTumorState.T2) primaryTumorState += "T2 = 20 mm but ≤50 mm";
            else if (state == PrimaryTumorState.T3) primaryTumorState += "T3 = 50 mm";
            else if (state == PrimaryTumorState.T4) primaryTumorState += "T4 = tumor of any size with direct extension to the chest wall and/or skin";

            return primaryTumorState;
        }
        public static string TranslateMetastatsisState(DistantMetastasisState state)
        {
            var distantMetastasisState = "";
            if (state == DistantMetastasisState.M0) distantMetastasisState += "M0 no evidence of distant metastases";
            else if (state == DistantMetastasisState.M1) distantMetastasisState += "M2 distant detectable metastases as determined by clinical and radiographic means";

            return distantMetastasisState;
        }
        public static string TranslateRegionalLymphNodesState(RegionalLymphNodesState state)
        {
            var lymphNodesState = "";
            if (state == RegionalLymphNodesState.N0) lymphNodesState += "N0 no regional lymph node metastases";
            else if (state == RegionalLymphNodesState.N1) lymphNodesState += "N1 metastases to moveable ipsilateral axillary lymph nodes";
            else if (state == RegionalLymphNodesState.N2) lymphNodesState += "N2 metastases in ipsilateral axillary lymph nodes that are clinically fixed";
            else if (state == RegionalLymphNodesState.N3) lymphNodesState += "N3 metastases that are more extensive";

            return lymphNodesState;
        }
        
        public static string GetTreatmentDisplayName(
            DiagnosedSurgery s,
            DiagnosedRadiation r,
            DiagnosedChemeotherapy c,
            DiagnosedEndocrineTreatment e)
        {
            var surgery = "";
            var radiation = "";
            var chemeotherapy = "";
            var endocrine = "";

            if (s != DiagnosedSurgery.S4) surgery += "Sugery";
            if (r != DiagnosedRadiation.R0) radiation += "Radiation";
            if (c != DiagnosedChemeotherapy.C1) chemeotherapy += "Chemeotherapy";
            if (e != DiagnosedEndocrineTreatment.E0) endocrine += "EndocrineTreatment";

            var arr = new string[] { surgery, radiation, chemeotherapy, endocrine };
            return string.Join("+", arr.Where(x => !string.IsNullOrEmpty(x)));
        }

        public static string GetTreatmentDescription(
            DiagnosedSurgery s,
            DiagnosedRadiation r,
            DiagnosedChemeotherapy c,
            DiagnosedEndocrineTreatment e)
        {
            var surgery = "";
            var radiation = "";
            var chemeotherapy = "";
            var endocrine = "";

            if (s != DiagnosedSurgery.S4) surgery += TranslateSurgery(s);
            if (r != DiagnosedRadiation.R0) radiation += TranslateRadiation(r);
            if (c != DiagnosedChemeotherapy.C1) chemeotherapy += TranslateChemeotherapy(c);
            if (e != DiagnosedEndocrineTreatment.E0) endocrine += TranslateEndocrineTreatment(e);

            var arr = new string[] {surgery, radiation, chemeotherapy, endocrine };
            
            return string.Join(Environment.NewLine, arr.Where(x=> !string.IsNullOrEmpty(x)));
        }


        private static string TranslateSurgery(DiagnosedSurgery state)
        {
            var surgeryState = "";
            if (state == DiagnosedSurgery.S1) surgeryState += "S1 Total mastectomy ± sentinel node biopsy ± reconstruction; or lumpectomy without lymph node surgery.";
            else if (state == DiagnosedSurgery.S2) surgeryState += "S2 Total mastectomy or lumpectomy + axillary staging ± breast reconstruction.";
            else if (state == DiagnosedSurgery.S3) surgeryState += "S3 If response to pre-operative therapy, total mastectomy or lumpectomy + axillary dissection ± delayed breast reconstruction.";
            else if (state == DiagnosedSurgery.S4) surgeryState += "S4 None";

            return surgeryState;
        }
        
        private static string TranslateRadiation(DiagnosedRadiation state)
        {
            var endocrineTreatmentState = "";
            if (state == DiagnosedRadiation.R1) endocrineTreatmentState += "R1 Whole breast radiation may be added to lumpectomy.";
            else if (state == DiagnosedRadiation.R2) endocrineTreatmentState += "R2 Radiation to whole breast and lymph nodes if involved; follows chemotherapy if provided.";
            else if (state == DiagnosedRadiation.R3) endocrineTreatmentState += "R3 Radiation to chest wall and lymph nodes.";
            else if (state == DiagnosedRadiation.R4) endocrineTreatmentState += "R4 Selective radiation to bone or brain metastases.";

            return endocrineTreatmentState;
        }

        private static string TranslateChemeotherapy(DiagnosedChemeotherapy state)
        {
            var chemeotherapyState = "";
            if (state == DiagnosedChemeotherapy.C1) chemeotherapyState += "C1 None";
            else if (state == DiagnosedChemeotherapy.C2) chemeotherapyState += "C2 Systemic adjuvant therapy as indicated by ER, PR, and HER2 status and predictive tests for chemotherapy benefit.";
            else if (state == DiagnosedChemeotherapy.C3) chemeotherapyState += "C3 Pre-operative systemic therapy";
            else if (state == DiagnosedChemeotherapy.C4) chemeotherapyState += "C4 If bone disease present, denosumab, zoledronic acid, or pamidronate.";

            return chemeotherapyState;
        }

        private static string TranslateEndocrineTreatment(DiagnosedEndocrineTreatment state)
        {
            var endocrineTreatmentState = "";
            if (state == DiagnosedEndocrineTreatment.E1) endocrineTreatmentState += "E1 If ER-positive, consider tamoxifen for 5 years for prevention.";
            else if (state == DiagnosedEndocrineTreatment.E2) endocrineTreatmentState += "E2 If ER-positive, tamoxifen for 10 years or aromatase inhibitor for 5 years (if post-menopausal only) or switching strategy of tamoxifen/aromatase inhibitor.";
            else if (state == DiagnosedEndocrineTreatment.E3) endocrineTreatmentState += "E3 If ER positive, consider ovarian ablation/ suppression for premenopausal women";
            else if (state == DiagnosedEndocrineTreatment.E4) endocrineTreatmentState += "E4 Treatment regimen based on receptor status";

            return endocrineTreatmentState;
        }

    }
}
