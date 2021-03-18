const initState = () => ({
  uploadPromise: null
})

export const state = initState;

export const mutations = {
  setTask(state, {promise}) {
    state.uploadPromise = promise
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  startVideoUpload({commit, dispatch}, {formData}) {
    // .post returns the whole promise, .$post returns promise.data
    const promise = this.$axios.$post("http://localhost:5500/api/videos", formData);
    commit("setTask", {promise})
  }
}
