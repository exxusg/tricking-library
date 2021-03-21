import {UPLOAD_TYPE} from "@/data/enum";

const initState = () => ({
  uploadPromise: null,
  active: false,
  type: "",
  step: 1
})

export const state = initState;

export const mutations = {
  toggleActivity(state) {
    state.active = !state.active;
    if(!state.active){
      Object.assign(state, initState())
    }
  },
  setType(state, type){
    state.type = type;
    if(type === UPLOAD_TYPE.TRICK){
      state.step++;
    }
    else if(type === UPLOAD_TYPE.SUBMISSION)
      state.step += 2;
  },
  incStep(state){
    state.step++;
  },
  setTask(state, {promise}) {
    state.uploadPromise = promise;
    state.step++;
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  startVideoUpload({commit, dispatch}, {formData}) {
    // .post returns the whole promise, .$post returns promise.data
    const promise = this.$axios.$post("/api/videos", formData);
    commit("setTask", {promise})
  },
  async createTrick({state, commit, dispatch}, {trick, submission}) {
    if (state.type === UPLOAD_TYPE.TRICK) {
      const createdTrick = await this.$axios.$post('/api/tricks', trick)
      submission.trickId = createdTrick.id;
    }
    
    const createdSubmission = await this.$axios.$post('/api/submissions', submission)
    
    await dispatch("tricks/fetchTricks", null, { root: true })
    await dispatch("submission/fetchSubmissions", null, { root: true })
  }
}
