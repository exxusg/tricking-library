<template>
  <item-content-layout>
    <template v-slot:content>
      <div v-if="submissions">
        <v-card v-for="s in submissions" :key="`video-card-${s.id}`">
          <v-card-text>{{ "Description: " + s.description }}</v-card-text>
          <video-player :video="s.video"></video-player>
        </v-card>
      </div>
    </template>
    <template v-slot:item>
      <v-sheet class="pa-2 sticky">
        <div class="text-h6 d-flex align-center">
          <span class="mr-2">{{ trick.name }}</span>
          <v-chip :to="`/difficulty/${difficulty.id}`">{{ difficulty.name }}</v-chip>
        </div>
        <v-divider class="my-2"></v-divider>
        <div class="text-body-2">{{ trick.description }}</div>
        <div class="text-subtitle-1 my-2"></div>

        <v-divider class="my-2"></v-divider>

        <div v-for="rd in relatedData" v-if="rd.data.length > 0">
          <div class="text-subtitle-1">{{ rd.title }}</div>
          <v-chip-group>
            <v-chip v-for="d in rd.data" :key="rd.idFactory(d)" :to="rd.routeFactory(d)">
              {{ d.name }}
            </v-chip>
          </v-chip-group>
        </div>
      </v-sheet>
    </template>
  </item-content-layout>
</template>

<script>
import {mapGetters, mapState} from 'vuex';
import VideoPlayer from "@/components/video-player";
import ItemContentLayout from "@/components/item-content-layout";

export default {
  components: {ItemContentLayout, VideoPlayer},
  data: () => ({
    
  }),
  computed: {
    ...mapState('submissions', ['submissions']),
    ...mapState('tricks', ['categories', 'tricks']),
    ...mapGetters('tricks', ['trickById', 'difficultyById']),
    trick() {
      return this.trickById(this.$route.params.trick)
    },
    difficulty() {
      return this.difficultyById(this.trick.difficulty)
    },
    relatedData() {
      return [
        {
          title: "Categories",
          data: this.categories.filter(x => this.trick.categories.indexOf(x.id) >= 0),
          idFactory: d => `category-${d.id}`,
          routeFactory: d => `/category/${d.id}`,
        },
        {
          title: "Prerequisites",
          data: this.tricks.filter(x => this.trick.prerequisites.indexOf(x.id) >= 0),
          idFactory: d => `pre-trick-${d.id}`,
          routeFactory: d => `/trick/${d.id}`,
        },
        {
          title: "Progressions",
          data: this.tricks.filter(x => this.trick.progressions.indexOf(x.id) >= 0),
          idFactory: d => `pro-trick-${d.id}`,
          routeFactory: d => `/trick/${d.id}`,
        }
      ]
    },
  },
  async fetch() {
    const trickId = this.$route.params.trick;
    await this.$store.dispatch("submissions/fetchSubmissionsForTricks", {trickId}, {root: true})
  },
  head() {
    return {
      title: this.trick.name,
      meta: [
        // hid is used as unique identifier. Do not use `vmid` for it as it will not work
        {
          hid: 'description',
          name: 'description',
          content: this.trick.description
        }
      ]
    }
  },
}
</script>

<style scoped>
  
</style>