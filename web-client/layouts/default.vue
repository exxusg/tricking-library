<template>
  <v-app>
    <v-app-bar app dense>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>

      <nuxt-link class="text--primary" style="text-decoration: none" to="/">
        <v-toolbar-title>Tricking library</v-toolbar-title>
      </nuxt-link>

      <v-spacer></v-spacer>
      
      <content-creation-dialog></content-creation-dialog>
    </v-app-bar>
    <v-main>
      <v-container>
        <nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import ContentCreationDialog from "@/components/content-creation/content-creation-dialog";

export default {
  components: {ContentCreationDialog}
}
</script>
