<template>
  <div>
    <div v-for="s in sections">
      <div class="d-flex flex-column align-center">
        <p class="text-h5">{{ s.title }}</p>
        <div>
          <v-btn class="mx-1" v-for="item in s.collection" 
                 :key="`${s.title}-${item.id}`"
                 :to="s.getRoute(item.id)">{{ item.name }}</v-btn>
        </div>
      </div>
      <v-divider class="my-5"></v-divider>
    </div>
  </div>
</template>

<script>
import {mapState} from 'vuex';

export default {
  components: {},
  data: () => ({
  }),
  computed: {
    ...mapState('tricks', ["tricks", "categories", "difficulties"]),
    sections() {
      return [
        {collection: this.tricks, title: "Tricks", getRoute: (id) => `/trick/${id}`},
        {collection: this.categories, title: "Categories", getRoute: (id) => `/category/${id}`},
        {collection: this.difficulties, title: "Difficulties", getRoute: (id) => `/difficulty/${id}`},
      ]
    }
  },
  created() {
  }
}
</script>
