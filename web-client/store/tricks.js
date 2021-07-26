
const initState = () => ({
  tricks: [],
  categories: [],
  difficulties: []
})

export const state = initState;

export const getters = {
  trickById: state => (id) => state.tricks.find(x => x.id === id),
  categoryById: state => (id) => state.categories.find(x => x.id === id),
  difficultyById: state => (id) => state.difficulties.find(x => x.id === id),

  trickItems: state => state.tricks.map(t => ({
    text: t.name,
    value: t.id
  })),
  categoryItems: state => state.categories.map(t => ({
    text: t.name,
    value: t.id
  })),
  difficultyItems: state => state.difficulties.map(t => ({
    text: t.name,
    value: t.id
  }))
}

export const mutations = {
  setTricks(state, {tricks, difficulties, categories}) {
    state.tricks = tricks
    state.difficulties = difficulties
    state.categories = categories

  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  async fetchTricks({commit}) {
    const tricks = (await this.$axios.$get('/api/tricks'))
    const difficulties = (await this.$axios.$get('/api/difficulties'))
    const categories = (await this.$axios.$get('/api/categories'))
    commit("setTricks", {tricks, difficulties, categories})
  },
  createTrick({ commit }, {form}) {
    console.log(state.tricks)
    return this.$axios.post('/api/tricks', form)
  }
}
