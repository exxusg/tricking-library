<template>
  <div>
    <div v-if="tricks">
      <div v-for="t in tricks">
        <v-btn :to="`/tricks/${t.id}`">{{t.name}}</v-btn>
      </div>
    </div>
    
  </div>
</template>

<script>
import {mapState, mapActions, mapMutations} from 'vuex';

export default {
  components: {},
  data: () => ({
  }),
  computed: {
    ...mapState('tricks', ['tricks']),
  },
  created() {
  },
  async fetch() {
    await this.$store.dispatch("tricks/fetchTricks");
  }
}
</script>
