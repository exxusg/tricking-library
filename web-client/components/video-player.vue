<template>
  <div class="video-container">
    <div class="play-button" :class="{'hide' : playing}" @click="playing = !playing">
      <v-icon size="70px">mdi-play</v-icon>
    </div>
    <video 
        ref="video" width="400" 
        muted loop 
        :src="`http://localhost:5500/api/videos/${video.videoLink}`"
        :poster="`http://localhost:5500/api/videos/${video.thumbLink}`"
        preload="none"
    ></video>
  </div>
</template>

<script>
export default {
  name: "video-player",
  props: {
    video: {
      required: true,
      type: Object
    }
  },
  data: () => ({
    playing: false
  }),
  watch: {
    playing: function (newValue) {
      if(newValue){
        this.$refs.video.play()
      }
      else {
        this.$refs.video.pause()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  .video-container {
    display: flex;
    position: relative;
    width: 100%;

    border-bottom-left-radius: inherit;
    border-bottom-right-radius: inherit;
    
    .play-button {
      position: absolute;
      display: flex;
      justify-content: center;
      align-items: center;
      background-color: rgba(0,0,0,0.36);
      width: 100%;
      height: 100%;
      z-index: 2;

      // on the same level as the class containing it
      &.hide {
        opacity: 0;
      }
    }
    
    video {
      width: 100%;
      z-index: 1;

      border-bottom-left-radius: inherit;
      border-bottom-right-radius: inherit;
    }
  }
</style>