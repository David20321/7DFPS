﻿using UnityEngine;
using ExtentionUtil;

namespace GunSystemsV1 {
    [InclusiveAspects(GunAspect.SLIDE_LOCK_VISUAL)]
    public class SlideLockVisualSystem : GunSystemBase {
        SlideLockVisualComponent slvc;

        public override void Initialize() {
            slvc = gs.GetComponent<SlideLockVisualComponent>();
        
            slvc.rel_pos = slvc.slide_lock.localPosition;
            slvc.rel_rot = slvc.slide_lock.localRotation;
        }

        public override void Update() {
            slvc.state = Mathf.Max(gs.IsSlideLocked() ? 1f : 0f, slvc.state - Time.deltaTime * 10.0f);

            slvc.slide_lock.LerpPosition(slvc.rel_pos, slvc.point_slide_locked, slvc.state);
            slvc.slide_lock.LerpRotation(slvc.rel_rot, slvc.point_slide_locked, slvc.state);
        }
    }

    [InclusiveAspects(GunAspect.HAMMER_VISUAL, GunAspect.HAMMER)]
    public class HammerVisualSystem : GunSystemBase {
        HammerVisualComponent hvc;
        HammerComponent hc;

        public override void Initialize() {
            hc = gs.GetComponent<HammerComponent>();
            hvc = gs.GetComponent<HammerVisualComponent>();

            hvc.hammer_rel_pos = hvc.hammer.localPosition;
            hvc.hammer_rel_rot = hvc.hammer.localRotation;
        }

        public override void Update() {
            hvc.hammer.LerpPosition(hvc.hammer_rel_pos, hvc.point_hammer_cocked, hc.hammer_cocked);
            hvc.hammer.LerpRotation(hvc.hammer_rel_rot, hvc.point_hammer_cocked, hc.hammer_cocked);
        }
    }

    [InclusiveAspects(GunAspect.TRIGGER_VISUAL, GunAspect.TRIGGER)]
    public class TriggerVisualSystem : GunSystemBase {
        TriggerVisualComponent tvc;
        TriggerComponent tc;

        public override void Initialize() {
            tc = gs.GetComponent<TriggerComponent>();
            tvc = gs.GetComponent<TriggerVisualComponent>();

            tvc.trigger_rel_pos = tvc.trigger.localPosition;
            tvc.trigger_rel_rot = tvc.trigger.localRotation;
        }

        public override void Update() {
            tvc.trigger.LerpPosition(tvc.trigger_rel_pos, tvc.point_trigger_pulled, tc.trigger_pressed);
            tvc.trigger.LerpRotation(tvc.trigger_rel_rot, tvc.point_trigger_pulled, tc.trigger_pressed);
        }
    }

    [InclusiveAspects(GunAspect.SLIDE, GunAspect.SLIDE_VISUAL)]
    public class SlideVisualSystem : GunSystemBase {
        SlideComponent psc;
        SlideVisualComponent svc;

        public override void Initialize() {
            psc = gs.GetComponent<SlideComponent>();
            svc = gs.GetComponent<SlideVisualComponent>();
        }

        public override void Update() {
            svc.slide.LerpPosition(svc.point_slide_start, svc.point_slide_end, psc.slide_amount);
        }
    }

    [InclusiveAspects(GunAspect.EXTRACTOR_ROD, GunAspect.EXTRACTOR_ROD_VISUAL)]
    public class ExtractorRodSystem : GunSystemBase {
        ExtractorRodVisualComponent ervc;
        ExtractorRodComponent erc;

        public override void Initialize() {
            ervc = gs.GetComponent<ExtractorRodVisualComponent>();
            erc = gs.GetComponent<ExtractorRodComponent>();

            ervc.extractor_rod_rel_pos = ervc.extractor_rod.localPosition;
            ervc.extractor_rod_rel_rot = ervc.extractor_rod.localRotation;
        }

        public override void Update() {
            ervc.extractor_rod.LerpPosition(ervc.extractor_rod_rel_pos, ervc.point_extractor_rod_extended, erc.extractor_rod_amount);
            ervc.extractor_rod.LerpRotation(ervc.extractor_rod_rel_rot, ervc.point_extractor_rod_extended, erc.extractor_rod_amount);
        }
    }

    [InclusiveAspects(GunAspect.REVOLVER_CYLINDER, GunAspect.CYLINDER_VISUAL)]
    public class CylinderVisualSystem : GunSystemBase {
        CylinderVisualComponent cvc;
        RevolverCylinderComponent rcc;

        public override void Initialize() {
            cvc = gs.GetComponent<CylinderVisualComponent>();
            rcc = gs.GetComponent<RevolverCylinderComponent>();
        }

        public override void Update() {
            if(rcc.rotateable) {
                var tmp_cs1 = cvc.cylinder_assembly.localRotation;
                var tmp_cs2 = tmp_cs1.eulerAngles;
                tmp_cs2.z = rcc.cylinder_rotation;

                tmp_cs1.eulerAngles = tmp_cs2;
                cvc.cylinder_assembly.localRotation = tmp_cs1;
            }
        }
    }

    [InclusiveAspects(GunAspect.YOKE, GunAspect.YOKE_VISUAL)]
    public class YokeVisualSystem : GunSystemBase {
        YokeVisualComponent yvc;
        YokeComponent yc;

        public override void Initialize() {
            yvc = gs.GetComponent<YokeVisualComponent>();
            yc = gs.GetComponent<YokeComponent>();

            yvc.yoke_pivot_rel_pos = yvc.yoke_pivot.localPosition;
            yvc.yoke_pivot_rel_rot = yvc.yoke_pivot.localRotation;
        }

        public override void Update() {
            yvc.yoke_pivot.LerpPosition(yvc.yoke_pivot_rel_pos, yvc.point_yoke_pivot_open, yc.yoke_open);
            yvc.yoke_pivot.LerpRotation(yvc.yoke_pivot_rel_rot, yvc.point_yoke_pivot_open, yc.yoke_open);
        }
    }
}