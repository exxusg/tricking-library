<template>
  <div>
    <div v-if="tricks">
      <div v-for="t in tricks">
        {{t.id}} - {{t.name}}
      </div>
    </div>
    <div v-if="submissions">
      <div v-for="s in submissions">
        {{s.id}} - {{"Description: " + s.description}} - {{s.trickId}}
        <div>
          <video width="400" controls :src="`http://localhost:5500/api/videos/${s.video}`"></video>
        </div>
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
    ...mapState('submission', ['submissions']),
  },
  created() {
  }
}
</script>
