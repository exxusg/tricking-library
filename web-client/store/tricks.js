﻿import axios from "axios";

const initState = () => ({
  tricks: []
})

export const state = initState;

export const mutations = {
  setTricks(state, {tricks}) {
    state.tricks = tricks
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  async fetchTricks({commit}) {
    const tricks = (await axios.get('http://localhost:5500/api/tricks')).data
    console.log("Tricks: ", tricks)
    commit("setTricks", {tricks})
  },
  async createTrick({commit, dispatch}, {trick}) {
    await axios.post('http://localhost:5500/api/tricks', trick)
    await dispatch("fetchTricks")
  }
}
