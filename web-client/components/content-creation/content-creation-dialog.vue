<template>
  <v-dialog :value="active" persistent width="700">
    <template v-slot:activator="{on}">
      <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
          <v-btn color="primary" dark v-bind="attrs" v-on="on">
            Create
          </v-btn>
        </template>
        <v-list>
          <v-list-item v-for="(item, index) in menuItems" :key="`ccd-menu-${index}`" 
                       @click="activate({component: item.component})">
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </template>
    
    <div v-if="component">
      <component :is="component"></component>
    </div>
    
<!--    <v-btn @click="cancelUpload">Close</v-btn>-->
  </v-dialog>
</template>

<script>
import {mapActions, mapMutations, mapState} from "vuex";
import TrickSteps from "@/components/content-creation/trick-steps";
import SubmissionSteps from "@/components/content-creation/submission-steps";
import DifficultyForm from "@/components/content-creation/difficulty-form";
import CategoryForm from "@/components/content-creation/category-form";

export default {
  name: "content-creation-dialog",
  components: {CategoryForm, DifficultyForm, SubmissionSteps, TrickSteps},
  computed: {
    ...mapState('video-upload', ['active', 'component']),
    menuItems() {
      return [
        {component: TrickSteps, title: "Trick"},
        {component: SubmissionSteps, title: "Submission"},
        {component: DifficultyForm, title: "Difficulty"},
        {component: CategoryForm, title: "Category"},
      ]
    }
  },
  methods: {
    ...mapMutations('video-upload', ['activate']),
    ...mapActions('video-upload', ['cancelUpload']),
  }
}
</script>

<style scoped>

</style>