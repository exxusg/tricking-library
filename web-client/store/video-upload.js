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
    console.log("Type: ", type);
    state.type = type;
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
  async createTrick({commit, dispatch}, {trick}) {
    await this.$axios.post('http://localhost:5500/api/tricks', trick)
    await dispatch("tricks/fetchTricks", null, { root: true })
  }
}
