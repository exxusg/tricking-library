<template>
  <item-content-layout>
    <template v-slot:content>
      <trick-list :tricks="tricks" class="mx-2"></trick-list>
    </template>
    <template v-slot:item>
      <v-sheet class="pa-2 sticky" v-if="category">
        <div class="text-h6">{{ category.name }}</div>
        <v-divider class="my-2"></v-divider>
        <div class="text-body-2">{{ category.description }}</div>
      </v-sheet>
    </template>
  </item-content-layout>
</template>

<script>
import {mapGetters, mapState} from "vuex";
import TrickList from "@/components/trick-list";
import ItemContentLayout from "@/components/item-content-layout";

export default {
  components: {ItemContentLayout, TrickList},
  data: () => ({
    tricks: [],
    filter: ""
  }),
  computed: {
    ...mapGetters('tricks', ['categoryById']),
    category() {
      return this.categoryById(this.$route.params.category);
    },
    filteredTricks() {
      if(!this.filter) return this.tricks
      
      const normalizedFilter = this.filter.trim().toLowerCase();
      
      return this.tricks.filter(t => t.name.toLowerCase().includes(normalizedFilter) 
          || t.description.toLowerCase().includes(normalizedFilter))
      
    }
  },
  async fetch() {
    const categoryId = this.$route.params.category;
    this.tricks = await this.$axios.$get(`/api/categories/${categoryId}/tricks`)
  },
  head() {
    return {
      title: this.category.name,
      meta: [
        // hid is used as unique identifier. Do not use `vmid` for it as it will not work
        {
          hid: 'description',
          name: 'description',
          content: this.category.description
        }
      ]
    }
  },
}
</script>

<style scoped>
  
</style>