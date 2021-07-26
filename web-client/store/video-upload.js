import {UPLOAD_TYPE} from "@/data/enum";

const initState = () => ({
  uploadPromise: null,
  uploadCancelSource: null,
  uploadCompleted: false,
  active: false,
  component: null
})

export const state = initState;

export const mutations = {
  activate(state, {component}) {
    state.active = true;
    state.component = component;
  },
  hide(state) {
    state.active = false;
  },
  setTask(state, {promise, source}) {
    state.uploadPromise = promise;
    state.uploadCancelSource = source;
  },
  completeUpload(state) {
    state.uploadCompleted = true;
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  startVideoUpload({commit, dispatch}, {formData}) {
    // .post returns the whole promise, .$post returns promise.data
    const source = this.$axios.CancelToken.source()
    const promise = this.$axios.post("/api/videos", formData, {
        cancelToken: source.token,
        progress: false
      }).then(({data}) => {
        commit('completeUpload')
        return data
      }).catch(err => {
        if(this.$axios.isCancel(err)){
          // todo popup notify
        }
        
      });
    
    //source.cancel()
    commit("setTask", {promise, source})
  },
  async cancelUpload({state, commit}){
    if(state.uploadPromise) {
      if(state.uploadCompleted) {
        commit('hide')
        const video = await state.uploadPromise
        await this.$axios.delete("/api/videos/" + video)
      }
      else {
        state.uploadCancelSource.cancel()
      }
    }
    
    commit('reset')
  },
  async createSubmission({state, commit, dispatch}, {form}) {
    if (!state.uploadPromise) {
      console.log("uploadPromise is null!")
      return;
    }
    
    form.video = await state.uploadPromise;
    await dispatch("submissions/createSubmission", {form}, {root: true})
    commit('reset')
  }
}
