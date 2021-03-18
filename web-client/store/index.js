import axios from "axios";

const initState = () => ({
  message: ""
})

export const state = initState;

export const mutations = {
  setMessage(state, message) {
    state.message = message
  },
  reset(state) {
    Object.assign(state, initState());
  }
}

export const actions = {
  async nuxtServerInit({commit, dispatch}) {
    const message = (await axios.get('http://localhost:5500/api/home')).data;
    commit("setMessage", message)
    await dispatch("tricks/fetchTricks")
  },
  async fetchMessage({commit}) {
    const message = (await axios.get('http://localhost:5500/api/home')).data;
    commit("setMessage", message)
  }
}
