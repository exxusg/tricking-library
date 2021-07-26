<template>
  <v-card>
    <v-card-title>
      Create submission
      <v-spacer></v-spacer>
      <v-btn icon @click="close">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-card-title>
    <v-stepper class="rounded-0" v-model="step">
      <v-stepper-header class="elevation-0">
        <v-stepper-step :complete="step > 1" step="1">
          Upload Video
        </v-stepper-step>

        <v-divider></v-divider>
        
        <v-stepper-step :complete="step > 2" step="2">
          Select Trick
        </v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="step > 3" step="3">
          Submission
        </v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step step="4">
          Review
        </v-stepper-step>
      </v-stepper-header>

      <v-stepper-items>
        <v-stepper-content class="pt-0" step="1">
          <div>
            <v-file-input accept="video/*" @change="handleFile"></v-file-input>
          </div>
        </v-stepper-content>
        
        <v-stepper-content class="pt-0" step="2">
          <div>
            <v-select :items="trickItems" v-model="form.trickId" label="Select Trick"></v-select>
            <v-btn @click="step++">Next</v-btn>
          </div>
        </v-stepper-content>

        <v-stepper-content class="pt-0" step="3">
          <div>
            <v-text-field label="Description" v-model="form.description"></v-text-field>
            <v-btn @click="step++">Next</v-btn>
          </div>
        </v-stepper-content>

        <v-stepper-content class="pt-0" step="4">
          <div>
            <v-btn @click="save">Save</v-btn>
          </div>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
  </v-card>
</template>

<script>
import {mapGetters, mapMutations, mapActions, mapState} from "vuex";
import {close} from "@/components/content-creation/_shared";

export default {
  name: "submission-steps",
  mixins: [close],
  data: () => ({
    step: 1,
    form: {
      trickId: "",
      video: "",
      description: ""
    }
  }),
  computed: {
    ...mapGetters('tricks', ['trickItems']),
  },
  methods: {
    ...mapMutations('video-upload', ['hide']),
    ...mapActions('video-upload', ['startVideoUpload', 'createSubmission']),
    handleFile(file) {
      if (!file) return;

      const formData = new FormData();
      formData.append("video", file);
      this.startVideoUpload({formData});
      this.step++;
    },
    save() {
      this.createSubmission({form: this.form})
      
      this.hide();
      Object.assign(this.$data, initState())
    }
  }
}
</script>

<style scoped>

</style>