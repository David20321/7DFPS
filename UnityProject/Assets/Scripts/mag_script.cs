using UnityEngine;
using System;
using System.Collections.Generic;

public enum MagLoadStage {NONE, PUSHING_DOWN, ADDING_ROUND, REMOVING_ROUND, PUSHING_UP};

public class mag_script:MonoBehaviour{
    
    int num_rounds = 8;
    public int kMaxRounds = 8;
    Vector3[] round_pos;
    Quaternion[] round_rot;
    Vector3 old_pos;
    public Vector3 hold_offset;
    public Vector3 hold_rotation;
    public List<AudioClip> sound_add_round;
    public List<AudioClip> sound_mag_bounce;

    public MagLoadStage mag_load_stage = MagLoadStage.NONE;
    public float mag_load_progress = 0.0f;
    public bool disable_interp = true;
    
    public bool RemoveRound() {
    	if(num_rounds == 0){
    		return false;
    	}
    	Transform round_obj = transform.Find("round_"+num_rounds);
    	round_obj.GetComponent<Renderer>().enabled = false;
    	--num_rounds;
    	return true;
    }
    
    public bool RemoveRoundAnimated() {
    	if(num_rounds == 0 || mag_load_stage != MagLoadStage.NONE){
    		return false;
    	}
    	mag_load_stage = MagLoadStage.REMOVING_ROUND;
    	mag_load_progress = 0.0f;
    	return true;
    }
    
    public bool IsFull() {
    	return num_rounds == kMaxRounds;
    }
    
    public bool AddRound() {
    	if(num_rounds >= kMaxRounds || mag_load_stage != MagLoadStage.NONE){
    		return false;
    	}
    	mag_load_stage = MagLoadStage.PUSHING_DOWN;
    	mag_load_progress = 0.0f;
    	PlaySoundFromGroup(sound_add_round, 0.3f);
    	++num_rounds;
    	Transform round_obj = transform.Find("round_"+num_rounds);
    	round_obj.GetComponent<Renderer>().enabled = true;
    	return true;
    }
    
    public int NumRounds() {
    	return num_rounds;
    }
    
    public void Start() {
    	old_pos = transform.position;
    	num_rounds = UnityEngine.Random.Range(0,kMaxRounds);
    	round_pos = new Vector3[kMaxRounds];
    	round_rot = new Quaternion[kMaxRounds];
    	for(int i=0; i<kMaxRounds; ++i){
    		Transform round = transform.Find("round_"+(i+1));
    		round_pos[i] = round.localPosition;
    		round_rot[i] = round.localRotation;
    		if(i < num_rounds){
    			round.GetComponent<Renderer>().enabled = true;
    		} else {
    			round.GetComponent<Renderer>().enabled = false;
    		}
    	}
    }
    
    public void PlaySoundFromGroup(List<AudioClip> group,float volume){
    	if(group.Count != 0){
    		int which_shot = UnityEngine.Random.Range(0,group.Count);
    		GetComponent<AudioSource>().PlayOneShot(group[which_shot], volume * PlayerPrefs.GetFloat("sound_volume", 1.0f));
    	}
    }
    
    public void Update() {
    	Transform obj = null;
        switch(mag_load_stage){
    		case MagLoadStage.PUSHING_DOWN:
    			mag_load_progress += Time.deltaTime * 20.0f;
    			if(mag_load_progress >= 1.0f){
    				mag_load_stage = MagLoadStage.ADDING_ROUND;
    				mag_load_progress = 0.0f;
    			}
    			break;
    		case MagLoadStage.ADDING_ROUND:
    			mag_load_progress += Time.deltaTime * 20.0f;
    			if(mag_load_progress >= 1.0f){
    				mag_load_stage = MagLoadStage.NONE;
    				mag_load_progress = 0.0f;
    				for(int i=0; i<num_rounds; ++i){
    					obj = transform.Find("round_"+(i+1));
    					obj.localPosition = round_pos[i];
    					obj.localRotation = round_rot[i];
    				}
    			}
    			break;
    		case MagLoadStage.PUSHING_UP:
    			mag_load_progress += Time.deltaTime * 20.0f;
    			if(mag_load_progress >= 1.0f){
    				mag_load_stage = MagLoadStage.NONE;
    				mag_load_progress = 0.0f;
    				RemoveRound();
    				for(int i=0; i<num_rounds; ++i){
    					obj = transform.Find("round_"+(i+1));
    					obj.localPosition = round_pos[i];
    					obj.localRotation = round_rot[i];
    				}
    			}
    			break;
    		case MagLoadStage.REMOVING_ROUND:
    			mag_load_progress += Time.deltaTime * 20.0f;
    			if(mag_load_progress >= 1.0f){
    				mag_load_stage = MagLoadStage.PUSHING_UP;
    				mag_load_progress = 0.0f;
    			}
    			break;
    	}
    	float mag_load_progress_display = mag_load_progress;
    	if(disable_interp){
    		mag_load_progress_display = Mathf.Floor(mag_load_progress + 0.5f);
    	}
    	switch(mag_load_stage){
    		case MagLoadStage.PUSHING_DOWN:
    			obj = transform.Find("round_1");
    			obj.localPosition = Vector3.Lerp(transform.Find("point_start_load").localPosition, 
    											 transform.Find("point_load").localPosition, 
    											 mag_load_progress_display);
    			obj.localRotation = Quaternion.Slerp(transform.Find("point_start_load").localRotation, 
    												 transform.Find("point_load").localRotation, 
    												 mag_load_progress_display);
    			for(int i=1; i<num_rounds; ++i){
    				obj = transform.Find("round_"+(i+1));
    				obj.localPosition = Vector3.Lerp(round_pos[i-1], round_pos[i], mag_load_progress_display);
    				obj.localRotation = Quaternion.Slerp(round_rot[i-1], round_rot[i], mag_load_progress_display);
    			}
    			break;
    		case MagLoadStage.ADDING_ROUND:
    			obj = transform.Find("round_1");
    			obj.localPosition = Vector3.Lerp(transform.Find("point_load").localPosition, 
    											 round_pos[0], 
    											 mag_load_progress_display);
    			obj.localRotation = Quaternion.Slerp(transform.Find("point_load").localRotation, 
    												 round_rot[0], 
    												 mag_load_progress_display);
    			for(int i=1; i<num_rounds; ++i){
    				obj = transform.Find("round_"+(i+1));
    				obj.localPosition = round_pos[i];
    			}
    			break;
    		case MagLoadStage.PUSHING_UP:
    			obj = transform.Find("round_1");
    			obj.localPosition = Vector3.Lerp(transform.Find("point_start_load").localPosition, 
    											 transform.Find("point_load").localPosition, 
    											 1.0f-mag_load_progress_display);
    			obj.localRotation = Quaternion.Slerp(transform.Find("point_start_load").localRotation, 
    												 transform.Find("point_load").localRotation, 
    												 1.0f-mag_load_progress_display);
    			for(int i=1; i<num_rounds; ++i){
    				obj = transform.Find("round_"+(i+1));
    				obj.localPosition = Vector3.Lerp(round_pos[i-1], round_pos[i], mag_load_progress_display);
    				obj.localRotation = Quaternion.Slerp(round_rot[i-1], round_rot[i], mag_load_progress_display);
    			}
    			break;
    		case MagLoadStage.REMOVING_ROUND:
    			obj = transform.Find("round_1");
    			obj.localPosition = Vector3.Lerp(transform.Find("point_load").localPosition, 
    											 round_pos[0], 
    											 1.0f-mag_load_progress_display);
    			obj.localRotation = Quaternion.Slerp(transform.Find("point_load").localRotation, 
    												 round_rot[0], 
    												 1.0f-mag_load_progress_display);
    			for(int i=1; i<num_rounds; ++i){
    				obj = transform.Find("round_"+(i+1));
    				obj.localPosition = round_pos[i];
    				obj.localRotation = round_rot[i];
    			}
    			break;
    	}
    }
}