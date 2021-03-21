<template>
  <v-app>
    <v-app-bar app dense>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>

      <v-toolbar-title>Tricking library</v-toolbar-title>

      <v-spacer></v-spacer>
      
      <video-upload></video-upload>
    </v-app-bar>
    <v-main>
      <v-container>
        <nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import { mapMutations } from 'vuex'
import VideoUpload from "~/components/video-upload";

export default {
  components: {VideoUpload},
  methods: {
    ...mapMutations('video-upload', ['toggleActivity'])
  },
  created() {
    this.$vuetify.theme.dark = true;
  }
}
</script>
