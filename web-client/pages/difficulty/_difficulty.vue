<template>
  <item-content-layout>
    <template v-slot:content>
      <trick-list :tricks="tricks" class="mx-2"></trick-list>
    </template>
    <template v-slot:item>
      <v-sheet class="pa-2 sticky" v-if="difficulty">
        <div class="text-h6">{{ difficulty.name }}</div>
        <v-divider class="my-2"></v-divider>
        <div class="text-body-2">{{ difficulty.description }}</div>
      </v-sheet>
    </template>
  </item-content-layout>
</template>

<script>
import {mapGetters} from "vuex";
import trickList from "@/mixins/trickList";
import TrickList from "@/components/trick-list";
import ItemContentLayout from "@/components/item-content-layout";

export default {
  components: {ItemContentLayout, TrickList},
  mixins: [trickList],
  data: () => ({
    tricks: [],
  }),
  computed: {
    ...mapGetters('tricks', ['difficultyById']),
    difficulty() {
      return this.difficultyById(this.$route.params.difficulty);
    },
  },
  async fetch() {
    const difficultyId = this.$route.params.difficulty;
    this.tricks = await this.$axios.$get(`/api/difficulties/${difficultyId}/tricks`)
  },
  head() {
    return {
      title: this.difficulty.name,
      meta: [
        // hid is used as unique identifier. Do not use `vmid` for it as it will not work
        {
          hid: 'description',
          name: 'description',
          content: this.difficulty.description
        }
      ]
    }
  },
}
</script>

<style scoped>

</style>