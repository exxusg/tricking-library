<template>
  <div>
    <div v-if="tricks">
      <div v-for="t in tricks">
        {{t.name}}
        <div>
          <video width="400" controls :src="`http://localhost:5500/api/videos/${t.video}`"></video>
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
    
  },
  created() {
  }
}
</script>
