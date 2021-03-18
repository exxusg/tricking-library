<template>

      <div class="text-xs-center">
        <logo />
        <vuetify-logo />
      </div>
      <div v-if="tricks">
        <p v-for="t in tricks">
          {{t.name}}
        </p>
      </div>
      <div>
        <v-text-field label="Trick name" v-model="trickName"></v-text-field>
        <v-btn @click="saveTrick">Save trick</v-btn>
      </div>
        {{ message }}
        <v-btn @click="reset">Reset message</v-btn>
      <v-btn @click="resetTricks">Reset trick</v-btn>
</template>

<script>
import Logo from '~/components/Logo.vue'
import VuetifyLogo from '~/components/VuetifyLogo.vue'
import { mapState, mapActions, mapMutations } from 'vuex'

export default {
  components: {
    Logo,
    VuetifyLogo
  },
  data: () => ({
    trickName: ''
  }),
  computed: {
    ...mapState({
      message: state => state.message
    }),
    ...mapState('tricks',{
      tricks: state => state.tricks
    })
  },
  methods: {
    ...mapMutations([
      'reset'
    ]),
    ...mapMutations('tricks', {
      resetTricks: 'reset'
    }),
    ...mapActions('tricks', ['createTrick']),
    async saveTrick() {
      await this.createTrick({trick: {name: this.trickName}});
    }
  },
  /*async fetch() {
    await this.$store.dispatch("fetchMessage");
  },*/
  /*asyncData({payload}){
    console.log("asyncData called");
    return axios.get('http://localhost:5500/api/home')
      //deconstructing data from the response
      .then(({data}) => {
        return {message: data}
      })
  },*/
  created() {}
}
</script>
