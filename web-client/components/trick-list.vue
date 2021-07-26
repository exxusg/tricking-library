<template>
  <div class="mx-2">
    <v-text-field v-model="filter" label="Search" outlined prepend-inner-icon="mdi-magnify"></v-text-field>
    <div v-for="t in filteredTricks">
      {{ t.name }} - {{ t.description }}
    </div>
  </div>
</template>

<script>
export default {
  name: "trick-list",
  props: {
    tricks: {
      required: true,
      type: Array
    }
  },
  data: () => ({
    filter: "",
  }),
  computed: {
    filteredTricks()
    {
      if (!this.filter) return this.tricks

      const normalizedFilter = this.filter.trim().toLowerCase();

      return this.tricks.filter(t => t.name.toLowerCase().includes(normalizedFilter)
          || t.description.toLowerCase().includes(normalizedFilter))

    }
  }
}
</script>

<style scoped>

</style>