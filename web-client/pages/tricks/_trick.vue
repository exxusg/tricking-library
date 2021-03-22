<template>
  <div class="d-flex justify-center align-start">
    <div class="mx-2" v-if="submissions">
      <div v-for="x in 12">
        <div v-for="s in submissions">
          {{ s.id }} - {{ "Description: " + s.description }} - {{ s.trickId }}
          <div>
            <video width="400" controls :src="`http://localhost:5500/api/videos/${s.video}`"></video>
          </div>
        </div>
      </div>
    </div>
    <div class="mx-2 sticky">
      Trick: {{ $route.params.trick }}
    </div>
  </div>
</template>

<script>
import {mapState} from 'vuex';

export default {
  computed: mapState('submission', ['submissions']),
  async fetch() {
    const trickId = this.$route.params.trick;
    await this.$store.dispatch("submission/fetchSubmissionsForTricks", {trickId}, {root: true})
  }
}
</script>

<style scoped>
  .sticky {
    position: -webkit-sticky;
    position: sticky;
    top: 48px
  }
</style>